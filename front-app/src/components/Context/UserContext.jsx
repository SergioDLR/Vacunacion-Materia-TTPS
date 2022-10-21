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
    if (userSesion?.idRol === 1) {
      navigate("/admin");
    }
    if (userSesion?.idRol === 2) {
      navigate("/analista");
    }
    if (userSesion?.idRol === 3) {
      navigate("/operador");
    }
  };

  useEffect(() => {
    const miStorage = window.localStorage;
    const session = JSON.parse(miStorage.getItem("session"));
    if (session !== null) {
      setUserSesion(session);
      if (session?.idRol === 1) {
        navigate("/admin");
      }
      if (session?.idRol === 2) {
        navigate("/analista");
      }
      if (session?.idRol === 3) {
        navigate("/operador");
      }
    }
  }, []);

  const cerrarSesion = () => {
    const miStorage = window.localStorage;
    miStorage.removeItem("session");
    setUserSesion({});
    navigate("/");
  };

  return <UserContext.Provider value={{ logIn, userSesion, cerrarSesion }}>{children}</UserContext.Provider>;
};

export default UserContextProvider;
