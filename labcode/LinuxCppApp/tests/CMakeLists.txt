add_executable(
    linux_cpp_app_tests
    test_calculator.cpp
    test_text_utils.cpp
)

target_link_libraries(
    linux_cpp_app_tests
    PRIVATE
        linux_cpp_app_core
        GTest::gtest_main
)

target_compile_features(
    linux_cpp_app_tests
    PRIVATE
        cxx_std_17
)

include(GoogleTest)

gtest_discover_tests(
    linux_cpp_app_tests
    DISCOVERY_TIMEOUT 30
)