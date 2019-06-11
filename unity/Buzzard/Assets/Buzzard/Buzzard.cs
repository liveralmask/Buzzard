using System;
using System.Runtime.InteropServices;

public class Buzzard {
  public delegate void BuzzardImageLoadTextureCallbackType( int id, int width, int height, IntPtr texture_data, int texture_data_size );
  
  [DllImport("Buzzard")]
  static public extern void buzzard_image_set_flip( bool value );
  static public void BuzzardImageSetFlip( bool value ){
    buzzard_image_set_flip( value );
  }
  
  [DllImport("Buzzard")]
  static public extern void buzzard_image_load_texture_data( int id, IntPtr image_data, int image_data_size, BuzzardImageLoadTextureCallbackType callback );
  static public unsafe void BuzzardImageLoadTextureData( int id, byte[] image_data, BuzzardImageLoadTextureCallbackType callback ){
    fixed( byte* image_data_p = image_data ){
      buzzard_image_load_texture_data( id, (IntPtr)image_data_p, image_data.Length, callback );
    }
  }
  
  static public byte[] ToBytes( IntPtr data, int data_size ){
    byte[] bytes = new byte[ data_size ];
    Marshal.Copy( data, bytes, 0, data_size );
    return bytes;
  }
  
  public delegate void ThreadCallbackType();
  
  static private System.Threading.SynchronizationContext s_MainThreadContext;
  
  static private System.Threading.Thread s_WorkerThread;
  static private System.Threading.SynchronizationContext s_WorkerThreadContext;
  
  static Buzzard(){
    BuzzardImageSetFlip( true );
  }
  
  static public void Begin(){
    if ( null == s_MainThreadContext ) s_MainThreadContext = System.Threading.SynchronizationContext.Current;
    s_WorkerThreadContext = null;
    
    s_WorkerThread = new System.Threading.Thread( () => {
      s_WorkerThreadContext = new System.Threading.SynchronizationContext();
      while ( null != s_WorkerThread ){
        System.Threading.Thread.Sleep( 1 );
      }
      s_WorkerThreadContext = null;
    } );
    s_WorkerThread.Start();
  }
  
  static public void End(){
    s_WorkerThread = null;
    s_MainThreadContext = null;
  }
  
  static public int GetThreadId(){
    return System.Threading.Thread.CurrentThread.ManagedThreadId;
  }
  
  static public void Thread( System.Threading.SynchronizationContext context, ThreadCallbackType callback ){
    context.Post( ( object state ) => {
      callback();
    }, null );
  }
  
  static public void MainThread( ThreadCallbackType callback ){
    Thread( s_MainThreadContext, callback );
  }
  
  static public void WorkerThread( ThreadCallbackType callback ){
    Thread( s_WorkerThreadContext, callback );
  }
}
