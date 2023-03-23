import axios from "axios";
import { apiResponse } from "../Interfaces";

const axiosInstance = axios.create({
  baseURL: "https://localhost:44315/api/",
  headers: {
    "Content-type": "application/json",
  },
});

export const registerUser = async (userData: any) => {
  try {
    const response = await axiosInstance.post("auth/register", userData);
    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const loginUser = async (userCredentials: any) => {
  try {
    const response = await axiosInstance.post("auth/login", userCredentials);
    return response.data;
  } catch (error) {
    console.error(error);
  }
};
