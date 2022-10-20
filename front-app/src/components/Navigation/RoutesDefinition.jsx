import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../Home";
import Login from "../Login/Login";
import HomeAdmin from "../Admin/HomeAdmin";
import UserListContainer from "../Users/UserListContainer";
import VacunasListContainer from "../OperadorNacional/vacunas/VacunasListContainer";
import AdminWraper from "../Nav/AdminWraper";
import OperadorWraper from "../OperadorNacional/OperadorWraper";
const RoutesDefinition = () => {
  return (
    <Routes>
      <Route path="*" element={<h1>Esta ruta no existe</h1>} />
      <Route path="/" element={<Home />} />
      <Route path="/login" element={<Login />} />
      <Route
        path="/admin"
        element={
          <ProtectedRoute role={1}>
            <AdminWraper>
              <HomeAdmin />
            </AdminWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/admin/users"
        element={
          <ProtectedRoute role={1}>
            <AdminWraper>
              <UserListContainer />
            </AdminWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/operador"
        element={
          <ProtectedRoute role={3}>
            <OperadorWraper>
              <h1>Bienvenido operador!</h1>
            </OperadorWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/operador/vacunas"
        element={
          <ProtectedRoute role={3}>
            <OperadorWraper>
              <VacunasListContainer />
            </OperadorWraper>
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default RoutesDefinition;
