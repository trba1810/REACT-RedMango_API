import React from "react";
import { useLocation } from "react-router-dom";
import { Elements } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import { PaymentForm } from "../Components/Page/Payment";
import { OrderSummary } from "../Components/Page/Order";

function Payment() {
  const {
    state: { apiResult, userInput },
  } = useLocation();
  const stripePromise = loadStripe(
    "pk_test_51MenYIHluzbSyBfpTPaP6VDy8ZTmLDNfRjLjLNqNPxR5XcOEvBpMU9dREA0qjZ911E9V5LOGoxVdgSdkRC1qEFQ300R4aFcFQ5"
  );
  const options = {
    clientSecret: apiResult.clientSecret,
  };

  return (
    <Elements stripe={stripePromise} options={options}>
      <div className="container m-5 p-5">
        <div className="row">
          <div className="col-md-7">
            <OrderSummary data={apiResult} userInput={userInput} />
          </div>
          <div className="col-md-4 offset-1">
            <h3 className="text-success">Payment</h3>
            <div className="mt-5">
              <PaymentForm />
            </div>
          </div>
        </div>
      </div>
    </Elements>
  );
}

export default Payment;
