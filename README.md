# Description
Enter your operation in the text input field and click on the "Calculate" button to let the application do its thing. 
API key is stored in app settings. I've implemented a middleware to validate the API key.

# Supports
1. Shows error when dividing by 0.

# Does not support
1. Brackets.
2. Certain (-) integer scenarios.

# Test Cases
Test case #1: 9 * 4 - 3 + 6 / 2 + 7 * 2 - 8
Solution: 42

Test case #2: 8 / 4 * 0 / 0 + 6
Solution: error (division by zero)
