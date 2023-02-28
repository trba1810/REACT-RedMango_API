import React from "react";
import { Header, Footer } from "../Components/Layout";
import { useState, useEffect } from "react";
import { menuItemModel } from "../Interfaces";
import { Home } from "../Pages";

function App() {
  return (
    <div>
      <Header />
      <Home />
      <Footer />
    </div>
  );
}

export default App;
