#ifndef LINUXCPPAPP_TEXT_UTILS_H
#define LINUXCPPAPP_TEXT_UTILS_H

#include <cstddef>
#include <string>

namespace linuxcppapp {

std::string to_upper(const std::string& text);

std::string reverse_text(const std::string& text);

std::size_t count_words(const std::string& text);

bool is_palindrome(const std::string& text);

}  // namespace linuxcppapp

#endif  // LINUXCPPAPP_TEXT_UTILS_H