import { createContext, useState } from "react";
import { useNavigate } from "react-router-dom";
export const UserContext = createContext();
const UserContextProvider = ({ children }) => {
  const [userSesion, setUserSesion] = useState({});
  const navigate = useNavigate();
  const logIn = (userSesion) => {
    setUserSesion(userSesion);
    if (userSesion.role === "admin") {
      navigate("/admin");
    }
  };
  const getRole = () => {
    return userSesion.role;
  };
  return <UserContext.Provider value={{ logIn, getRole }}>{children}</UserContext.Provider>;
};

export default UserContextProvider;
