namespace ElevatorSimulation.Transactions
{
    /// <summary>
    /// Model of the tenant
    /// </summary>
    class Tenant
    {
        public int Id { get; set; }
        /// <summary>
        /// Starting floor
        /// </summary>
        public int FloorFrom { get; set; }
        /// <summary>
        /// Destination floor
        /// </summary>
        public int FloorTo { get; set; }
        
        public Tenant(int id, int floorFrom, int floorTo)
        {
            Id = id;
            FloorFrom = floorFrom;
            FloorTo = floorTo;
        }   
        /// <summary>
        /// Get request from external controls
        /// </summary>
        /// <returns></returns>
        public Request GetExternalRequest()
        {
            return new Request(FloorFrom, GetRequestType());
        }
        /// <summary>
        /// Get request from internal controls
        /// </summary>
        /// <returns></returns>
        public Request GetInternalRequest()
        {
            return new Request(FloorTo, GetRequestType());
        }
        public RequestType GetRequestType()
        {
            if (FloorFrom < FloorTo)
            {
                return RequestType.Up;
            }
            else
            {
                return RequestType.Down;
            }
        }
    }
}
