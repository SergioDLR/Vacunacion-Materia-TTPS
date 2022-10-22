import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../Home";
import Login from "../Login/Login";
import HomeAdmin from "../Admin/HomeAdmin";
import UserListContainer from "../Users/UserListContainer";
import VacunasListContainer from "../OperadorNacional/vacunas/VacunasListContainer";
import VacunasDesarrolladasContainer from "../OperadorNacional/VacunasDesarrolladas/VacunasDesarrolladasContainer";
import AdminWraper from "../Nav/AdminWraper";
import OperadorWraper from "../OperadorNacional/OperadorWraper";
import AnalistaWraper from "../AnalistaProvincial/AnalistaWraper";
import MarcasComercialesContainer from "../OperadorNacional/MarcasComerciales/MarcasComercialesContainer";
const RoutesDefinition = () => {
  return (
    <Routes>
      <Route path="*" element={<Home />} />
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

      <Route
        path="/operador/vacunasdesarrolladas"
        element={
          <ProtectedRoute role={3}>
            <OperadorWraper>
              <VacunasDesarrolladasContainer />
            </OperadorWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/operador/marcascomerciales"
        element={
          <ProtectedRoute role={3}>
            <OperadorWraper>
              <MarcasComercialesContainer />
            </OperadorWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/analista"
        element={
          <ProtectedRoute role={2}>
            <AnalistaWraper>
              <h1>Bienvenido analista!</h1>
            </AnalistaWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/analista/vacunas"
        element={
          <ProtectedRoute role={2}>
            <AnalistaWraper>
              <h1>Pagina de vacunas</h1>
            </AnalistaWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/analista/vacunados"
        element={
          <ProtectedRoute role={2}>
            <AnalistaWraper>
              <h1>Pagina de vacunados</h1>
            </AnalistaWraper>
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default RoutesDefinition;
