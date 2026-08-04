cmake_minimum_required(VERSION 3.20)

project(
    LinuxCppApp
    VERSION 1.0.0
    DESCRIPTION "Simple Linux C++ application for a Jenkins workshop"
    LANGUAGES CXX
)

include(GNUInstallDirs)

option(
    BUILD_TESTING
    "Build the unit tests"
    ON
)

add_library(
    linux_cpp_app_core
    src/calculator.cpp
    src/text_utils.cpp
)

target_include_directories(
    linux_cpp_app_core
    PUBLIC
        ${PROJECT_SOURCE_DIR}/include
)

target_compile_features(
    linux_cpp_app_core
    PUBLIC
        cxx_std_17
)

if(MSVC)
    target_compile_options(
        linux_cpp_app_core
        PRIVATE
            /W4
    )
else()
    target_compile_options(
        linux_cpp_app_core
        PRIVATE
            -Wall
            -Wextra
            -Wpedantic
    )
endif()

add_executable(
    linux-cpp-app
    src/main.cpp
)

target_link_libraries(
    linux-cpp-app
    PRIVATE
        linux_cpp_app_core
)

target_compile_features(
    linux-cpp-app
    PRIVATE
        cxx_std_17
)

install(
    TARGETS linux-cpp-app
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
)

install(
    FILES README.md
    DESTINATION ${CMAKE_INSTALL_DOCDIR}
)

if(BUILD_TESTING)
    include(CTest)

    find_package(GTest CONFIG QUIET)

    if(NOT GTest_FOUND)
        include(FetchContent)

        FetchContent_Declare(
            googletest
            URL
                https://github.com/google/googletest/archive/refs/tags/v1.15.2.zip
        )

        if(MSVC)
            set(
                gtest_force_shared_crt
                ON
                CACHE BOOL
                ""
                FORCE
            )
        endif()

        FetchContent_MakeAvailable(googletest)
    endif()

    add_subdirectory(tests)
endif()

set(CPACK_PACKAGE_NAME "LinuxCppApp")
set(CPACK_PACKAGE_VENDOR "Jenkins Workshop")
set(CPACK_PACKAGE_DESCRIPTION_SUMMARY
    "Simple Linux C++ command-line application"
)
set(CPACK_PACKAGE_VERSION ${PROJECT_VERSION})
set(CPACK_GENERATOR "TGZ")

include(CPack)