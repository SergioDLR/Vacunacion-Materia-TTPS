import { Container } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useEffect, useState, useContext } from "react";
import axios from "axios";
import allUrls from "@/services/backend_url";
import { UserContext } from "@/components/Context/UserContext";
import { useAlert } from "react-alert";
import MarcasComercialesTable from "./MarcasComercialesTable";
import CargarMarcaComercial from "./CargarMarcaComercial";
import { cargarMarcas } from "@/services/getMarcasComerciales";
const MarcasComercialesContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [marcasComerciales, setMarcasComerciales] = useState([]);
  const alert = useAlert();
  useEffect(() => {
    cargarMarcasComerciales();
  }, []);

  const cargarMarcasComerciales = () => {
    cargarMarcas(setMarcasComerciales, allUrls.marcasComerciales, userSesion.email, alert);
  };
  return (
    <Container>
      <CargarMarcaComercial cargarMarcasComerciales={cargarMarcasComerciales} />
      <MarcasComercialesTable marcasComerciales={marcasComerciales} cargarMarcasComerciales={cargarMarcasComerciales} />
    </Container>
  );
};

export default MarcasComercialesContainer;
