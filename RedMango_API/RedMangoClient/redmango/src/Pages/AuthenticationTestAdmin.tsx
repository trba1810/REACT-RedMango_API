import React from "react";
import { withAdminAuth } from "../HOC";

function AuthenticationTestAdmin() {
  return <div>only admin</div>;
}

export default withAdminAuth(AuthenticationTestAdmin);
