using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;

public class ImageSample : MonoBehaviour {
  static private RawImage s_RawImage;
  
  void Awake(){
    s_RawImage = GameObject.Find( "RawImage" ).GetComponent< RawImage >();
    
    Buzzard.Begin();
  }
  
  void OnDestroy(){
    Buzzard.End();
  }
  
  void Start(){
    // TODO
    string url = "https://raw.githubusercontent.com/liveralmask/UnitySamples/master/Profiles/WebSampleProfile.png";
    StartCoroutine( Get( url ) );
  }
  
  private IEnumerator Get( string url ){
    UnityWebRequest request = UnityWebRequest.Get( url );
    yield return request.SendWebRequest();
    if ( request.isNetworkError || request.isHttpError ){
      UnityEngine.Debug.LogError( request.error );
      EditorApplication.isPlaying = false;
    }else{
      byte[] data = request.downloadHandler.data;
      // TODO
#if true
      Buzzard.WorkerThread( () => {
        Buzzard.BuzzardImageLoadTextureData( 0, (byte[])data, OnLoadTextureData );
      } );
#else
      Texture2D texture = new Texture2D( 0, 0, TextureFormat.RGBA32, false );
      ImageConversion.LoadImage( texture, (byte[])data, false );
      SetTexture( s_RawImage, texture );
#endif
    }
  }
  
  [AOT.MonoPInvokeCallback(typeof(Buzzard.BuzzardImageLoadTextureCallbackType))]
  static private void OnLoadTextureData( int id, int width, int height, IntPtr texture_data_p, int texture_data_size ){
    byte[] texture_data = Buzzard.ToBytes( texture_data_p, texture_data_size );
    Buzzard.MainThread( () => {
      Texture2D texture = new Texture2D( width, height, TextureFormat.RGBA32, false );
      texture.LoadRawTextureData( texture_data );
      texture.Apply( false, false );
      SetTexture( s_RawImage, texture );
    } );
  }
  
  static private void SetTexture( RawImage raw_image, Texture texture ){
    raw_image.texture = texture;
    raw_image.GetComponent< RectTransform >().sizeDelta = new Vector2( texture.width, texture.height );
  }
}
