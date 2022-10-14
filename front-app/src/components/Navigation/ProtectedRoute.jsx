import { useContext, useState } from "react";
import { UserContext } from "../Context/UserContext";
import { Navigate } from "react-router-dom";
const ProtectedRoute = ({ children, role }) => {
  const { getRole } = useContext(UserContext);
  const [authRole] = useState(() => getRole());
  if (role === authRole) {
    return children;
  } else {
    return <Navigate to="/" replace={true} />;
  }
};

export default ProtectedRoute;
