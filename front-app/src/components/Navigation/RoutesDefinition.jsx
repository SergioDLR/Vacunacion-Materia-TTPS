import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../Home";
import Login from "../Login/Login";
import UserListContainer from "../Users/UserListContainer";
import VacunasListContainer from "../OperadorNacional/vacunas/VacunasListContainer";
import VacunasDesarrolladasContainer from "../OperadorNacional/VacunasDesarrolladas/VacunasDesarrolladasContainer";
import AdminWraper from "../Nav/AdminWraper";
import OperadorWraper from "../OperadorNacional/OperadorWraper";
import AnalistaWraper from "../AnalistaProvincial/AnalistaWraper";
import MarcasComercialesContainer from "../OperadorNacional/MarcasComerciales/MarcasComercialesContainer";
import HomeOperador from "../OperadorNacional/Home";
import AplicarVacuna from "../Vacunador/AplicarVacuna";
import VacunadorWraper from "../Vacunador/VacunadorWrapper";
import Vacunados from "../AnalistaProvincial/VacunadosInfo/Vacunados";
import CompraContainer from "../OperadorNacional/Compras/CompraContainer";
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
              <HomeOperador mensaje={"administrador"} />
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
              <HomeOperador mensaje={"operador nacional"} />
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
              <HomeOperador mensaje={"analista"} />
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
              <HomeOperador mensaje={"vacunador"} />
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
      <Route
        path="/operador/compras"
        element={
          <ProtectedRoute role={3}>
            <OperadorWraper>
              <CompraContainer />
            </OperadorWraper>
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default RoutesDefinition;
