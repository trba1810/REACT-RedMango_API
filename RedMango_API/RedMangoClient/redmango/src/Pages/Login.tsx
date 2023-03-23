import React, { useState } from "react";
import { useLoginUserMutation } from "../Apis/authApi";
import { inputHelper } from "../Helper";
import { apiResponse } from "../Interfaces";
import { loginUser } from "../Apis/LogRegApi";

function Login() {
  const [error, setError] = useState("");
  // const [loginUser] = useLoginUserMutation();
  const [loading, setLoading] = useState(false);
  const [userInput, setUserInput] = useState({
    username: "",
    password: "",
  });

  const handleUserInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    const tempData = inputHelper(e, userInput);
    setUserInput(tempData);
  };
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);
    const response: apiResponse = await loginUser({
      username: userInput.username,
      password: userInput.password,
    });
    console.log(response);
    //   setError(response.error);
    // setLoading(false);
  };
  return (
    <div className="container text-center">
      <form method="POST" onSubmit={handleSubmit}>
        <h1 className="mt-5">Login</h1>
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
              type="password"
              className="form-control"
              placeholder="Enter Password"
              required
              name="password"
              value={userInput.password}
              onChange={handleUserInput}
            />
          </div>
        </div>

        <div className="mt-2">
          {error && <p className="text-warning">{error}</p>}
          <button
            type="submit"
            className="btn btn-success"
            style={{ width: "200px" }}
          >
            Login
          </button>
        </div>
      </form>
    </div>
  );
}

export default Login;
