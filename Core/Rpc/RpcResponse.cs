﻿using System;
using Newtonsoft.Json;

namespace IgiCore.Core.Rpc
{
	public class RpcResponse<T> : RpcMessage
	{
		private readonly Lazy<T> result;

		public T Result => this.result.Value;

		public RpcResponse()
		{
			this.result = new Lazy<T>(() => JsonConvert.DeserializeObject<T>(this.Payloads[0]));
		}

		public static RpcResponse<T> Parse(string result)
		{
			return JsonConvert.DeserializeObject<RpcResponse<T>>(result);
		}
	}

	public class RpcResponse<T1, T2> : RpcMessage
	{
		private readonly Lazy<T1> result1;
		private readonly Lazy<T2> result2;

		public T1 Result1 => this.result1.Value;
		public T2 Result2 => this.result2.Value;

		public RpcResponse()
		{
			this.result1 = new Lazy<T1>(() => JsonConvert.DeserializeObject<T1>(this.Payloads[0]));
			this.result2 = new Lazy<T2>(() => JsonConvert.DeserializeObject<T2>(this.Payloads[1]));
		}

		public static RpcResponse<T1, T2> Parse(string result)
		{
			return JsonConvert.DeserializeObject<RpcResponse<T1, T2>>(result);
		}
	}

	public class RpcResponse<T1, T2, T3> : RpcMessage
	{
		private readonly Lazy<T1> result1;
		private readonly Lazy<T2> result2;
		private readonly Lazy<T3> result3;

		public T1 Result1 => this.result1.Value;
		public T2 Result2 => this.result2.Value;
		public T3 Result3 => this.result3.Value;

		public RpcResponse()
		{
			this.result1 = new Lazy<T1>(() => JsonConvert.DeserializeObject<T1>(this.Payloads[0]));
			this.result2 = new Lazy<T2>(() => JsonConvert.DeserializeObject<T2>(this.Payloads[1]));
			this.result3 = new Lazy<T3>(() => JsonConvert.DeserializeObject<T3>(this.Payloads[2]));
		}

		public static RpcResponse<T1, T2, T3> Parse(string result)
		{
			return JsonConvert.DeserializeObject<RpcResponse<T1, T2, T3>>(result);
		}
	}

	public class RpcResponse<T1, T2, T3, T4> : RpcMessage
	{
		private readonly Lazy<T1> result1;
		private readonly Lazy<T2> result2;
		private readonly Lazy<T3> result3;
		private readonly Lazy<T4> result4;

		public T1 Result1 => this.result1.Value;
		public T2 Result2 => this.result2.Value;
		public T3 Result3 => this.result3.Value;
		public T4 Result4 => this.result4.Value;

		public RpcResponse()
		{
			this.result1 = new Lazy<T1>(() => JsonConvert.DeserializeObject<T1>(this.Payloads[0]));
			this.result2 = new Lazy<T2>(() => JsonConvert.DeserializeObject<T2>(this.Payloads[1]));
			this.result3 = new Lazy<T3>(() => JsonConvert.DeserializeObject<T3>(this.Payloads[2]));
			this.result4 = new Lazy<T4>(() => JsonConvert.DeserializeObject<T4>(this.Payloads[3]));
		}

		public static RpcResponse<T1, T2, T3, T4> Parse(string result)
		{
			return JsonConvert.DeserializeObject<RpcResponse<T1, T2, T3, T4>>(result);
		}
	}

	public class RpcResponse<T1, T2, T3, T4, T5> : RpcMessage
	{
		private readonly Lazy<T1> result1;
		private readonly Lazy<T2> result2;
		private readonly Lazy<T3> result3;
		private readonly Lazy<T4> result4;
		private readonly Lazy<T5> result5;

		public T1 Result1 => this.result1.Value;
		public T2 Result2 => this.result2.Value;
		public T3 Result3 => this.result3.Value;
		public T4 Result4 => this.result4.Value;
		public T5 Result5 => this.result5.Value;

		public RpcResponse()
		{
			this.result1 = new Lazy<T1>(() => JsonConvert.DeserializeObject<T1>(this.Payloads[0]));
			this.result2 = new Lazy<T2>(() => JsonConvert.DeserializeObject<T2>(this.Payloads[1]));
			this.result3 = new Lazy<T3>(() => JsonConvert.DeserializeObject<T3>(this.Payloads[2]));
			this.result4 = new Lazy<T4>(() => JsonConvert.DeserializeObject<T4>(this.Payloads[3]));
			this.result5 = new Lazy<T5>(() => JsonConvert.DeserializeObject<T5>(this.Payloads[4]));
		}

		public static RpcResponse<T1, T2, T3, T4, T5> Parse(string result)
		{
			return JsonConvert.DeserializeObject<RpcResponse<T1, T2, T3, T4, T5>>(result);
		}
	}
}
