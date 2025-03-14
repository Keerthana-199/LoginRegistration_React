import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import Register from './Register'
import Login from './Login'
import Dashboard from './Dashboard'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { isAuthenticated } from "./utils/auth";

const PrivateRoute = ({ children }) => {
  return isAuthenticated() ? children : <Navigate to="/login" />;
};
function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div className='app'>
        {/* <Register></Register><br>
        </br> */}
        {/* <Router>
        <Login></Login>
        </Router> */}
      </div>
      <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
         <Route path="/dashboard" element={<PrivateRoute><Dashboard /></PrivateRoute>} />
        <Route path="/" element={<Navigate to="/login" />} />
        </Routes>
    </Router> 
    </>
  )
}

export default App
