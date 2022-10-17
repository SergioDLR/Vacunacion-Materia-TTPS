import { useContext, useState } from "react";
import { UserContext } from "../Context/UserContext";
import { Navigate } from "react-router-dom";
const ProtectedRoute = ({ children, role }) => {
  const { userSesion } = useContext(UserContext);
  if (role === userSesion.idRol) {
    return children;
  } else {
    return <Navigate to="/" replace={true} />;
  }
};

export default ProtectedRoute;
