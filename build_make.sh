#!/bin/bash

rm -f CMakeCache.txt
cmake . $*
make clean all

case "$(uname)" in
Darwin)
  if [ ! -f libBuzzard.dylib ]; then
    exit 1
  fi
  
  cp libBuzzard.dylib unity/Buzzard/Assets/Buzzard/Plugins/Buzzard.bundle
  ;;
esac
