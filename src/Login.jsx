import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
  
      const response = await axios.post("https://localhost:7156/api/users/login", 
        { email: email, password: password }, 
        { headers: { "Content-Type": "application/json" } }
    )
    console.log(response);
      // await axios.post("https://localhost:7078/api/users/login", { 
      //   method: "POST",
      // headers: { "Content-Type": "application/json" },
      // body: JSON.stringify({ email, password }),
      //  });
      
       const data = await response.data;
      console.log(data);
       if (response.status === 200) {
         sessionStorage.setItem("jwtToken", data.tokenInfo.token);
         sessionStorage.setItem("UserDetails", JSON.stringify(data.tokenInfo.user));
         alert("Login successful!");
         navigate("/dashboard"); 
       } else {
         alert(data.message || "Login failed");
       }
  };

  return (
    <div>
      <h2>Login</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <form onSubmit={handleLogin}>
        <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        <button type="submit">Login</button>
        <div>Not a user? <a href="/Register">Register here</a></div>
      </form>
    </div>
  );
};

export default Login;
