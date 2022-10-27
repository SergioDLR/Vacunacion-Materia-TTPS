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
import HomeOperador from "../OperadorNacional/home";
import HomeAnalista from "../AnalistaProvincial/home";
import AplicarVacuna from "../Vacunador/AplicarVacuna";
import VacunadorWraper from "../Vacunador/VacunadorWrapper";
import Vacunados from "../AnalistaProvincial/VacunadosInfo/Vacunados";
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
              <HomeOperador />
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
              <HomeAnalista />
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
              <Vacunados />
            </AnalistaWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/vacunador"
        element={
          <ProtectedRoute role={4}>
            <VacunadorWraper>
              <HomeOperador />
            </VacunadorWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/vacunador/aplicar"
        element={
          <ProtectedRoute role={4}>
            <VacunadorWraper>
              <AplicarVacuna />
            </VacunadorWraper>
          </ProtectedRoute>
        }
      />
      <Route
        path="/operador/vacunados"
        element={
          <ProtectedRoute role={3}>
            <OperadorWraper>
              <Vacunados />
            </OperadorWraper>
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default RoutesDefinition;
