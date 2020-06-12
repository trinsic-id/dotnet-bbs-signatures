# BBS+ for .NET Core

This is a .NET wrapper for the C callable BBS+ Signatures package located https://github.com/mikelodder7/ffi-bbs-signatures
The are pre-built dynamic libraries avaliable for each platform in the `libs/` folder. Follow the instructions below if you'd like to build the dependency youself.
This wrapper handles automatic memory management when working with unmanaged memory.

# Requirements

- [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1 or newer
- Optionally, if you'd like to build the BBS+ library yourself
  - Install [Rust](https://www.rust-lang.org/tools/install)
  - Follow installation instructions at https://github.com/mikelodder7/ffi-bbs-signatures

# Demo

There's a full [end-to-end integration test](https://github.com/streetcred-id/bbs-signatures-dotnet/blob/mac-debug/src/BbsSignatures.Tests/BbsIntegrationTests.cs) available that showcases the use of each of the library methods.

# Roadmap

- Support for [BBS+ signature schemes using JSON-LD](https://github.com/mattrglobal/bbs-signatures) credentials (Q3, 2020)
- Support for WASM runtime using browser wallet (Q4, 2020)