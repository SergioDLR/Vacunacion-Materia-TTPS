import { Container, Box } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useEffect, useState, useContext } from "react";
import axios from "axios";
import allUrls from "@/services/backend_url";
import { UserContext } from "@/components/Context/UserContext";
import { useAlert } from "react-alert";
import MarcasComercialesTable from "./MarcasComercialesTable";
import CargarMarcaComercial from "./CargarMarcaComercial";
import CustomLoader from "@/components/utils/CustomLoader";
import { cargarMarcas } from "@/services/getMarcasComerciales";
const MarcasComercialesContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [marcasComerciales, setMarcasComerciales] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const alert = useAlert();
  useEffect(() => {
    cargarMarcasComerciales();
  }, []);

  const cargarMarcasComerciales = () => {
    setEstaCargando(true);
    cargarMarcas(setMarcasComerciales, allUrls.marcasComerciales, userSesion.email, alert, handleCarga);
  };

  const handleCarga = () => {
    setEstaCargando(false);
  };
  return (
    <Container>
      {estaCargando ? (
        <CustomLoader />
      ) : (
        <>
          <Box sx={{ marginTop: 2, marginBottom: 2 }}>
            <CargarMarcaComercial cargarMarcasComerciales={cargarMarcasComerciales} />
          </Box>
          <MarcasComercialesTable
            marcasComerciales={marcasComerciales}
            cargarMarcasComerciales={cargarMarcasComerciales}
          />
        </>
      )}
    </Container>
  );
};

export default MarcasComercialesContainer;
