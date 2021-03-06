﻿using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Pathoschild.Http.FluentClient.Default
{
	/// <summary>Sends HTTP requests and receives responses from a resource identified by a URI.</summary>
	public class Client : IClient
	{
		/*********
		** Accessors
		*********/
		/// <summary>The underlying HTTP client.</summary>
		public HttpClient BaseClient { get; set; }

		/// <summary>The formatters used for serializing and deserializing message bodies.</summary>
		public MediaTypeFormatterCollection Formatters { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="client">The underlying HTTP client.</param>
		public Client(HttpClient client)
		{
			this.BaseClient = client;
			this.Formatters = new MediaTypeFormatterCollection();
		}

		/// <summary>Construct an instance.</summary>
		/// <param name="baseUri">The base URI prepended to relative request URIs.</param>
		public Client(string baseUri)
			: this(new HttpClient { BaseAddress = new Uri(baseUri) })
		{ }

		/// <summary>Create an asynchronous HTTP DELETE request message (but don't dispatch it yet).</summary>
		/// <param name="resource">The URI to send the request to.</param>
		/// <returns>Returns a request builder.</returns>
		public IRequestBuilder Delete(string resource)
		{
			return this.Send(HttpMethod.Delete, resource);
		}

		/// <summary>Create an asynchronous HTTP GET request message (but don't dispatch it yet).</summary>
		/// <param name="resource">The URI to send the request to.</param>
		/// <returns>Returns a request builder.</returns>
		public IRequestBuilder Get(string resource)
		{
			return this.Send(HttpMethod.Get, resource);
		}

		/// <summary>Create an asynchronous HTTP POST request message (but don't dispatch it yet).</summary>
		/// <param name="resource">The URI to send the request to.</param>
		/// <returns>Returns a request builder.</returns>
		public IRequestBuilder Post(string resource)
		{
			return this.Send(HttpMethod.Post, resource);
		}

		/// <summary>Create an asynchronous HTTP POST request message (but don't dispatch it yet).</summary>
		/// <typeparam name="TBody">The request body type.</typeparam>
		/// <param name="resource">The URI to send the request to.</param>
		/// <param name="body">The request body.</param>
		/// <returns>Returns a request builder.</returns>
		public IRequestBuilder Post<TBody>(string resource, TBody body)
		{
			return this.Post(resource).WithBody(body);
		}

		/// <summary>Create an asynchronous HTTP PUT request message (but don't dispatch it yet).</summary>
		/// <param name="resource">The URI to send the request to.</param>
		/// <returns>Returns a request builder.</returns>
		public IRequestBuilder Put(string resource)
		{
			return this.Send(HttpMethod.Put, resource);
		}

		/// <summary>Create an asynchronous HTTP PUT request message (but don't dispatch it yet).</summary>
		/// <typeparam name="TBody">The request body type.</typeparam>
		/// <param name="resource">The URI to send the request to.</param>
		/// <param name="body">The request body.</param>
		/// <returns>Returns a request builder.</returns>
		public IRequestBuilder Put<TBody>(string resource, TBody body)
		{
			return this.Put(resource).WithBody(body);
		}

		/// <summary>Create an asynchronous request message (but don't dispatch it yet).</summary>
		/// <param name="method">The HTTP method.</param>
		/// <param name="resource">The URI to send the request to.</param>
		/// <returns>Returns a request builder.</returns>
		public virtual IRequestBuilder Send(HttpMethod method, string resource)
		{
			Uri uri = new Uri(this.BaseClient.BaseAddress, resource);
			HttpRequestMessage message = new HttpRequestMessage(method, uri);
			return this.Send(message);
		}

		/// <summary>Create an asynchronous request message (but don't dispatch it yet).</summary>
		/// <param name="message">The HTTP request message to send.</param>
		/// <returns>Returns a request builder.</returns>
		/// <remarks>This is the base method which executes every request.</remarks>
		public virtual IRequestBuilder Send(HttpRequestMessage message)
		{
			return new RequestBuilder(message, this.Formatters, request => this.BaseClient.SendAsync(request.Message));
		}
	}
}
