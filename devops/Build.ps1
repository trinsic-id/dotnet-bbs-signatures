
param($Platform, $OutLocation)

git clone https://github.com/mikelodder7/ffi-bbs-signatures.git
Set-Location ffi-bbs-signatures

switch ($platform) {
    windows {
        cargo build --release 
        Copy-Item -Path .\target\release\bbs.dll -Destination $OutLocation
        break
    }
    linux { 
        cargo build --release 
        Copy-Item -Path .\target\release\libbbs.so -Destination $OutLocation
        break
    }
    macos {
        cargo build --release 
        Copy-Item -Path .\target\release\libbbs.dylib -Destination $OutLocation
        break
    }
    ios {
        cargo install cargo-lipo
        rustup target install x86_64-apple-ios aarch64-apple-ios
        cargo lipo --release
        Copy-Item -Path .\target\universal\libbbs.a -Destination $OutLocation
        break
    }
    android {
        cargo lipo --release
        Copy-Item -Path .\target\release\libbbs.a -Destination $OutLocation
        break
    }
}