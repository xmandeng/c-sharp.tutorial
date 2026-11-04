#!/usr/bin/env bash
# Run the tests with code coverage and build an HTML report in coverage-report/.
# Python equivalent: pytest --cov && coverage html
set -euo pipefail
cd "$(dirname "$0")/.."

dotnet tool restore >/dev/null
rm -rf TestResults coverage-report
dotnet test --coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml
dotnet reportgenerator \
    -reports:"TestResults/coverage.cobertura.xml" \
    -targetdir:coverage-report \
    -reporttypes:"Html;TextSummary" \
    -assemblyfilters:"+Tutorial*;-Tutorial.Tests"

sed -n '1,12p' coverage-report/Summary.txt
echo "Open coverage-report/index.html in a browser for line-by-line detail."
