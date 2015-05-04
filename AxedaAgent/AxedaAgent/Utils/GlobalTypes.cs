// -----------------------------------------------------------------------------
//           $Copyright: (C) Copyright by Bayer Healthcare 2013. All rights reserved. $
// -----------------------------------------------------------------------------
// 
//  $Source: AxedaAgent/Utils/GlobalTypes.cs $
//  $Revision: 1.1 $
//  $Date: 2012/09/12 14:43:42EDT $
//  $Author: MLHAY $
//  $ProjectName: g:/MKS_Files/Service_Engineering/ApplicationFramework/AxedaAgent/project.pj $
// 
// -----------------------------------------------------------------------------
namespace Medrad.ServiceEngineering.ApplicationFramework.Axeda.Agent
{
    /// <summary>
    /// Agent read response structure
    /// </summary>
    public struct AgentReadResponse
    {
        /// <summary>
        /// Gets or sets the property read status returned by the agent.
        /// </summary>
        public AgentDataReadStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the value returned by the agent.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="agentReadResponse1">The agent read response1.</param>
        /// <param name="agentReadResponse2">The agent read response2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AgentReadResponse agentReadResponse1, AgentReadResponse agentReadResponse2)
        {
            return agentReadResponse1.Equals(agentReadResponse2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="agentReadResponse1">The agent read response1.</param>
        /// <param name="agentReadResponse2">The agent read response2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(AgentReadResponse agentReadResponse1, AgentReadResponse agentReadResponse2)
        {
            return !agentReadResponse1.Equals(agentReadResponse2);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Status.GetHashCode() * 397) ^ (this.Value != null ? this.Value.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is AgentReadResponse && this.Equals((AgentReadResponse)obj);
        }

        /// <summary>
        /// Equals the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns> The comparison result</returns>
        public bool Equals(AgentReadResponse other)
        {
            return AgentReadResponse.Equals(other.Status, this.Status) && AgentReadResponse.Equals(other.Value, this.Value);
        }
    }
}
