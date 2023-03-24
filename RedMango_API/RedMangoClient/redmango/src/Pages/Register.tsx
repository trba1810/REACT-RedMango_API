import React, { useState } from "react";
import { useRegisterUserMutation } from "../Apis/authApi";
import { inputHelper } from "../Helper";
import { apiResponse } from "../Interfaces";
import { SD_Roles } from "../Utility/SD";
import { registerUser } from "../Apis/LogRegApi";

function Register() {
  // const [registerUser] = useRegisterUserMutation();

  const [loading, setLoading] = useState(false);
  const [userInput, setUserInput] = useState({
    username: "",
    password: "",
    role: "",
    name: "",
  });

  const handleUserInput = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const tempData = inputHelper(e, userInput);
    setUserInput(tempData);
  };

  const handleSubmitForm = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);

    const response: apiResponse = await registerUser({
      username: userInput.username,
      password: userInput.password,
      role: userInput.role,
      name: userInput.name,
    });
    console.log(response);
  };

  // This code is getting HTTP 400 error

  // const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
  //   e.preventDefault();
  //   setLoading(true);

  //   let userName = userInput.username;
  //   let uPassword = userInput.password;
  //   let uRole = userInput.role;
  //   let uName = userInput.name;
  //   console.log(uName);
  //   console.log(uPassword);
  //   console.log(userName);
  //   console.log(uRole);

  //   const response: apiResponse = await registerUser({
  //     username: userInput.username,
  //     password: userInput.password,
  //     role: userInput.role,
  //     name: userInput.name,
  //   });
  //   console.log(response);
  //   if (response.data) {
  //     console.log(response.data);
  //   } else if (response.error) {
  //     console.log(response.data);
  //   }
  //   setLoading(false);
  // };

  return (
    <div className="container text-center">
      <form method="post" onSubmit={handleSubmitForm}>
        <h1 className="mt-5">Register</h1>
        <div className="mt-5">
          <div className="col-sm-6 offset-sm-3 col-xs-12 mt-4">
            <input
              type="text"
              className="form-control"
              placeholder="Enter Username"
              required
              name="username"
              value={userInput.username}
              onChange={handleUserInput}
            />
          </div>
          <div className="col-sm-6 offset-sm-3 col-xs-12 mt-4">
            <input
              type="text"
              className="form-control"
              placeholder="Enter Name"
              required
              name="name"
              value={userInput.name}
              onChange={handleUserInput}
            />
          </div>
          <div className="col-sm-6 offset-sm-3 col-xs-12 mt-4">
            <input
              type="password"
              className="form-control"
              placeholder="Enter Password"
              required
              name="password"
              value={userInput.password}
              onChange={handleUserInput}
            />
          </div>
          <div className="col-sm-6 offset-sm-3 col-xs-12 mt-4">
            <select
              className="form-control form-select"
              required
              value={userInput.role}
              name="role"
              onChange={handleUserInput}
            >
              <option value="">--Select Role--</option>
              <option value={`${SD_Roles.CUSTOMER}`}>Customer</option>
              <option value={`${SD_Roles.ADMIN}`}>Admin</option>
            </select>
          </div>
        </div>
        <div className="mt-5">
          <button type="submit" className="btn btn-success">
            Register
          </button>
        </div>
      </form>
    </div>
  );
}

export default Register;
