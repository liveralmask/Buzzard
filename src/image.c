#include <buzzard.h>

#define STB_IMAGE_IMPLEMENTATION
#include "./stb_image.h"

typedef void BUZZARD_CC (*BuzzardImageLoadTextureCallbackType)( int32 id, int32 width, int32 height, void* texture_data, int32 texture_data_size );

BUZZARD_API void BUZZARD_CC buzzard_image_set_flip( bool value ){
  stbi_set_flip_vertically_on_load( value );
}

BUZZARD_API void BUZZARD_CC buzzard_image_load_texture_data( int32 id, void* image_data, int32 image_data_size, BuzzardImageLoadTextureCallbackType callback ){
  int width = 0;
  int height = 0;
  int comp = 0;
  int req_comp = 4/* RGBA */;
  void* texture_data = stbi_load_from_memory( image_data, image_data_size, &width, &height, &comp, req_comp );
  if ( null != texture_data ){
    callback( id, width, height, texture_data, width * height * req_comp );
    stbi_image_free( texture_data );
  }else{
    callback( id, 0, 0, null, 0 );
  }
}
