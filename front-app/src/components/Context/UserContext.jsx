import { createContext, useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
export const UserContext = createContext();
const UserContextProvider = ({ children }) => {
  const [userSesion, setUserSesion] = useState({});
  const navigate = useNavigate();

  const logIn = (session) => {
    const miStorage = window.localStorage;
    miStorage.setItem("session", JSON.stringify(session));
    setUserSesion(session);
    checkSesionNavigate(session);
  };

  useEffect(() => {
    const miStorage = window.localStorage;
    const session = JSON.parse(miStorage.getItem("session"));
    if (session !== null) {
      setUserSesion(session);
      checkSesionNavigate(session);
    }
  }, []);

  const checkSesionNavigate = (session) => {
    switch (session?.idRol) {
      case 1:
        navigate("/admin");
        break;
      case 2:
        navigate("/analista");
        break;
      case 3:
        navigate("/operador");
        break;
      case 4:
        navigate("/vacunador");
        break;
      default:
        console.log("hay algo raro en la sesion");
    }
  };

  const cerrarSesion = () => {
    const miStorage = window.localStorage;
    miStorage.removeItem("session");
    setUserSesion({});
    navigate("/");
  };

  return <UserContext.Provider value={{ logIn, userSesion, cerrarSesion }}>{children}</UserContext.Provider>;
};

export default UserContextProvider;
