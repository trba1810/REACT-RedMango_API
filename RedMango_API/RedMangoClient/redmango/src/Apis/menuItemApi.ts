import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const menuItemApi = createApi({
  reducerPath: "menuItemApi",
  baseQuery: fetchBaseQuery({
    baseUrl: "https://localhost:44315/api/",
  }),
  tagTypes: ["menuItems"],
  endpoints: (builder) => ({
    GetMenuItems: builder.query({
      query: () => ({
        url: "menuItem",
      }),
      providesTags: ["menuItems"],
    }),
    GetMenuItemsById: builder.query({
      query: (id) => ({
        url: `menuItem/${id}`,
      }),
      providesTags: ["menuItems"],
    }),
  }),
});

export const { useGetMenuItemsQuery, useGetMenuItemsByIdQuery } = menuItemApi;
export default menuItemApi;
