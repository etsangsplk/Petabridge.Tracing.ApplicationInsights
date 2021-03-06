﻿// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsSpanContext.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2018 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using OpenTracing;

namespace Petabridge.Tracing.ApplicationInsights
{
    /// <summary>
    ///     Describes the type of span being used.
    /// </summary>
    public enum SpanKind
    {
        CLIENT,
        SERVER
    }

    /// <summary>
    ///     Application Insights operation-specific context.
    /// </summary>
    public sealed class ApplicationInsightsSpanContext : IApplicationInsightsSpanContext
    {
        public ApplicationInsightsSpanContext(string traceId, string spanId,
            string parentId = null)
        {
            TraceId = traceId;
            SpanId = spanId;
            ParentId = parentId;
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            throw new NotSupportedException("Baggage is not supported in Application Insights.");
        }

        /// <inheritdoc />
        /// <summary>
        ///     Refers to the OperationId in Application Insights
        /// </summary>
        public string TraceId { get; }

        public string SpanId { get; }
        public string ParentId { get; }

        public bool Equals(IApplicationInsightsSpanContext other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(TraceId, other.TraceId)
                   && string.Equals(SpanId, other.SpanId)
                   && string.Equals(ParentId, other.ParentId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is IApplicationInsightsSpanContext && Equals((IApplicationInsightsSpanContext) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TraceId != null ? TraceId.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (SpanId != null ? SpanId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParentId != null ? ParentId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ApplicationInsightsSpanContext left, ApplicationInsightsSpanContext right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApplicationInsightsSpanContext left, ApplicationInsightsSpanContext right)
        {
            return !Equals(left, right);
        }
    }

    /// <summary>
    ///     Application Insights-specific <see cref="T:OpenTracing.ISpanContext" />.
    /// </summary>
    public interface IApplicationInsightsSpanContext : ISpanContext, IEquatable<IApplicationInsightsSpanContext>
    {
        /// <summary>
        ///     The operation_ParentId equivalent in the Application Insights data model.
        /// </summary>
        string ParentId { get; }
    }
}