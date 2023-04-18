import React from "react";

function MyOrders() {
  return (
    <div className="table p-5">
      <h1 className="text-success">Orders List</h1>
      <div className="p-2">
        <div className="row border">
          <div className="col-1">ID</div>
          <div className="col-3">Name</div>
          <div className="col-2">Phone</div>
          <div className="col-1">Total</div>
          <div className="col-1">Items</div>
          <div className="col-2">Date</div>
          <div className="col-2"></div>
        </div>
        <div className="row border">
          <div className="col-1">ID</div>
          <div className="col-3">NAME</div>
          <div className="col-2">PHONE</div>
          <div className="col-1">$ TOTAL</div>
          <div className="col-1"># ITEMS</div>
          <div className="col-2">DATE</div>
          <div className="col-2">
            <button className="btn btn-success">Details</button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default MyOrders;
