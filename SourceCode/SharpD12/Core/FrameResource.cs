﻿using SharpDX;
using SharpDX.Direct3D12;

namespace SharpD12
{
  public class FrameResource
  {
    public long fenceValue = 0;
    public CommandAllocator cmdAllocator;
    public Resource backBuffer;
    public CpuDescriptorHandle backBufferHandle;

    public static Resource depthBuffer;
    public static CpuDescriptorHandle dsvHandle;
    public static DescriptorHeap rtvDescHeap;
    public static DescriptorHeap dsvDescHeap;
    public static UploadBuffer<PassConstants> passBuffer;
    public static UploadBuffer<ObjectConstants> objectBuffer;
  }

  public struct PassConstants
  {
    public Matrix viewProj;
  }
}