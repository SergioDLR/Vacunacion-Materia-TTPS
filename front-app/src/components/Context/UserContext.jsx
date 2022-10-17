import { createContext, useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
export const UserContext = createContext();
const UserContextProvider = ({ children }) => {
  const [userSesion, setUserSesion] = useState({});
  const navigate = useNavigate();
  const logIn = (userSesion) => {
    const miStorage = window.localStorage;
    miStorage.setItem("session", JSON.stringify(userSesion));
    setUserSesion(userSesion);
    if (userSesion.role === 1) {
      navigate("/admin");
    }
  };
  useEffect(() => {
    const miStorage = window.localStorage;
    const session = miStorage.getItem("session");
    if (session !== null) {
      setUserSesion(JSON.parse(session));
      if (session.role === 1) {
        navigate("/admin");
      }
    }
  }, []);
  const getRole = () => {
    return userSesion.role;
  };
  return <UserContext.Provider value={{ logIn, getRole, userSesion }}>{children}</UserContext.Provider>;
};

export default UserContextProvider;
