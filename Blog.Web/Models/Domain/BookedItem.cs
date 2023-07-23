namespace Blog.Web.Models.Domain
{
    public class BookedItem
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ItemName { get; set; }

       
        // <div class="form-group">
          //      <label for="dateTimeInput">Select Date and Time:</label>
            //    <input type = "datetime-local" id="dateTimeInput">
              // </div>
        

    }

}
