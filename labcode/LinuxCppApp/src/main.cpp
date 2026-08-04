#include "linuxcppapp/calculator.h"
#include "linuxcppapp/text_utils.h"

#include <exception>
#include <iostream>
#include <stdexcept>
#include <string>

namespace {

void print_usage(const std::string& program_name) {
    std::cout
        << "LinuxCppApp - Simple C++ application with basic functions\n\n"
        << "Usage:\n"
        << "  " << program_name << " add <number1> <number2>\n"
        << "  " << program_name << " subtract <number1> <number2>\n"
        << "  " << program_name << " multiply <number1> <number2>\n"
        << "  " << program_name << " factorial <number>\n"
        << "  " << program_name << " even <number>\n"
        << "  " << program_name << " uppercase <text>\n"
        << "  " << program_name << " reverse <text>\n"
        << "  " << program_name << " words <text>\n"
        << "  " << program_name << " palindrome <text>\n";
}

int parse_integer(const std::string& value) {
    std::size_t processed_characters = 0;
    const int result = std::stoi(value, &processed_characters);

    if (processed_characters != value.length()) {
        throw std::invalid_argument(
            "The value is not a valid integer: " + value);
    }

    return result;
}

}  // namespace

int main(int argc, char* argv[]) {
    if (argc < 2) {
        print_usage(argv[0]);
        return 1;
    }

    const std::string command = argv[1];

    try {
        if (command == "add" && argc == 4) {
            const int first = parse_integer(argv[2]);
            const int second = parse_integer(argv[3]);

            std::cout << linuxcppapp::add(first, second) << '\n';
            return 0;
        }

        if (command == "subtract" && argc == 4) {
            const int first = parse_integer(argv[2]);
            const int second = parse_integer(argv[3]);

            std::cout << linuxcppapp::subtract(first, second) << '\n';
            return 0;
        }

        if (command == "multiply" && argc == 4) {
            const int first = parse_integer(argv[2]);
            const int second = parse_integer(argv[3]);

            std::cout << linuxcppapp::multiply(first, second) << '\n';
            return 0;
        }

        if (command == "factorial" && argc == 3) {
            const int value = parse_integer(argv[2]);

            std::cout << linuxcppapp::factorial(value) << '\n';
            return 0;
        }

        if (command == "even" && argc == 3) {
            const int value = parse_integer(argv[2]);

            std::cout
                << (linuxcppapp::is_even(value) ? "true" : "false")
                << '\n';

            return 0;
        }

        if (command == "uppercase" && argc == 3) {
            std::cout << linuxcppapp::to_upper(argv[2]) << '\n';
            return 0;
        }

        if (command == "reverse" && argc == 3) {
            std::cout << linuxcppapp::reverse_text(argv[2]) << '\n';
            return 0;
        }

        if (command == "words" && argc == 3) {
            std::cout << linuxcppapp::count_words(argv[2]) << '\n';
            return 0;
        }

        if (command == "palindrome" && argc == 3) {
            std::cout
                << (linuxcppapp::is_palindrome(argv[2])
                        ? "true"
                        : "false")
                << '\n';

            return 0;
        }

        std::cerr << "Invalid command or incorrect number of arguments.\n\n";
        print_usage(argv[0]);
        return 1;
    } catch (const std::exception& exception) {
        std::cerr << "Error: " << exception.what() << '\n';
        return 2;
    }
}