import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../Home";
import Login from "../Login/Login";
import HomeAdmin from "../Admin/HomeAdmin";
const RoutesDefinition = () => {
  return (
    <Routes>
      <Route path="*" element={<h1>Esta ruta no existe</h1>} />
      <Route path="/" element={<Home />} />
      <Route path="/login" element={<Login />} />
      <Route
        path="/admin"
        element={
          <ProtectedRoute role="admin">
            <HomeAdmin />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default RoutesDefinition;
