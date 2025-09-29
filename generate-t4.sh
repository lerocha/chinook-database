#!/bin/bash

# T4 Template Generation Script for Chinook Database
# This script generates all T4 templates using the t4 command line tool
# Prerequisites:
#   - t4 command line tool installed
#   - NuGet packages restored (dotnet restore)

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# NuGet package paths
NEWTONSOFT_JSON="$HOME/.nuget/packages/newtonsoft.json/13.0.3/lib/net6.0/Newtonsoft.Json.dll"
VISUALSTUDIO_INTEROP="$HOME/.nuget/packages/microsoft.visualstudio.interop/17.14.40260/lib/net8.0/Microsoft.VisualStudio.Interop.dll"

echo -e "${YELLOW}Chinook Database T4 Template Generation${NC}"
echo "========================================="

# Check if t4 command is available
if ! command -v t4 &> /dev/null; then
    echo -e "${RED}Error: t4 command not found. Please install the t4 command line tool.${NC}"
    echo "Install with: dotnet tool install -g dotnet-t4"
    exit 1
fi

# Check if NuGet packages exist
if [ ! -f "$NEWTONSOFT_JSON" ]; then
    echo -e "${RED}Error: Newtonsoft.Json package not found at $NEWTONSOFT_JSON${NC}"
    echo "Please run 'dotnet restore' to restore NuGet packages."
    exit 1
fi

if [ ! -f "$VISUALSTUDIO_INTEROP" ]; then
    echo -e "${RED}Error: Microsoft.VisualStudio.Interop package not found at $VISUALSTUDIO_INTEROP${NC}"
    echo "Please run 'dotnet restore' to restore NuGet packages."
    exit 1
fi

echo -e "${YELLOW}Step 1: Generating Chinook.ttinclude...${NC}"
t4 -r="$NEWTONSOFT_JSON" ChinookDatabase/_T4Templates/Chinook.tt
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Chinook.ttinclude generated successfully${NC}"
else
    echo -e "${RED}✗ Failed to generate Chinook.ttinclude${NC}"
    exit 1
fi

echo -e "${YELLOW}Step 2: Generating ChinookDatabase SQL scripts...${NC}"
t4 -r="$NEWTONSOFT_JSON" ChinookDatabase/DataSources/ChinookDatabase.tt
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ ChinookDatabase SQL scripts generated successfully${NC}"
else
    echo -e "${RED}✗ Failed to generate ChinookDatabase SQL scripts${NC}"
    exit 1
fi

echo -e "${YELLOW}Step 3: Generating DatabaseFixture test files...${NC}"
cd ChinookDatabase.Test/DatabaseTests
t4 -r="$NEWTONSOFT_JSON" -r="$VISUALSTUDIO_INTEROP" DatabaseFixture.tt
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ DatabaseFixture test files generated successfully${NC}"
else
    echo -e "${RED}✗ Failed to generate DatabaseFixture test files${NC}"
    cd ../..
    exit 1
fi
cd ../..

echo ""
echo -e "${GREEN}🎉 All T4 templates generated successfully!${NC}"
echo ""
echo "Generated files:"
echo "  - ChinookDatabase/_T4Templates/Chinook.ttinclude"
echo "  - ChinookDatabase/DataSources/Chinook_*.sql (multiple database scripts)"
echo "  - ChinookDatabase.Test/DatabaseTests/*Fixture.cs (test fixture classes)"
echo ""
echo "You can now build and test the project:"
echo "  dotnet build"
echo "  dotnet test"