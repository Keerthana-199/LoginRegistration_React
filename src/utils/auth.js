// Store token in session storage
export const setToken = (token) => {
    sessionStorage.setItem("jwtToken", token);
  };
  
  // Retrieve token from session storage
  export const getToken = () => {
    return sessionStorage.getItem("jwtToken");
  };
  
  // Remove token (for logout)
  export const removeToken = () => {
    sessionStorage.removeItem("jwtToken");
  };
  // Check if user is authenticated
export const isAuthenticated = () => {
    return !!getToken(); // Returns true if token exists, false otherwise
  };
  