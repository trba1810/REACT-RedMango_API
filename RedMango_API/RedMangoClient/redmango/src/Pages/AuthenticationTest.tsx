import React from "react";
import { withAuth } from "../HOC";

function AuthenticationTest() {
  return <div>Any logged in user</div>;
}

export default withAuth(AuthenticationTest);
