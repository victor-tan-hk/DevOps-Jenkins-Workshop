#include "linuxcppapp/calculator.h"

#include <stdexcept>

namespace linuxcppapp {

int add(const int first, const int second) {
    return first + second;
}

int subtract(const int first, const int second) {
    return first - second;
}

int multiply(const int first, const int second) {
    return first * second;
}

int factorial(const int value) {
    if (value < 0) {
        throw std::invalid_argument(
            "Factorial cannot be calculated for a negative number.");
    }

    int result = 1;

    for (int current = 2; current <= value; ++current) {
        result *= current;
    }

    return result;
}

bool is_even(const int value) {
    return value % 2 == 0;
}

}  // namespace linuxcppapp