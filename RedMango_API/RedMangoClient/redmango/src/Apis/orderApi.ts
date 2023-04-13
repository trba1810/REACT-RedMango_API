import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const orderApi = createApi({
  reducerPath: "orderApi",
  baseQuery: fetchBaseQuery({
    baseUrl: "https://localhost:44315/api/",
  }),
  tagTypes: ["MenuItems"],
  endpoints: (builder) => ({
    initiatePayment: builder.mutation({
      query: (orderDetails) => ({
        url: "order",
        method: "POST",
        headers: {
          "Content-type": "application/json",
        },
        body: orderDetails,
      }),
    }),
  }),
});

export const { useInitiatePaymentMutation } = orderApi;
export default orderApi;
