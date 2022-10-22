import { Container } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useEffect, useState, useContext } from "react";
import axios from "axios";
import allUrls from "@/services/backend_url";
import { UserContext } from "@/components/Context/UserContext";
import { useAlert } from "react-alert";
import MarcasComercialesTable from "./MarcasComercialesTable";

const MarcasComercialesContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [marcasComerciales, setMarcasComerciales] = useState([]);
  const alert = useAlert();
  useEffect(() => {
    cargarMarcasComerciales();
  }, []);

  const cargarMarcasComerciales = () => {
    try {
      axios.get(`${allUrls.marcasComerciales}?emailOperadorNacional=${userSesion.email}`).then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          setMarcasComerciales(response?.data?.listasMarcasComercialesDTO);
          console.log(response.data.listasMarcasComercialesDTO);
        } else {
          alert.error(response?.data?.errores);
        }
      });
    } catch (e) {
      alert.error("Ocurrio un error con el servidor");
    }
  };
  return (
    <Container>
      <CustomModal title="Cargar marca"></CustomModal>
      <MarcasComercialesTable marcasComerciales={marcasComerciales} cargarMarcasComerciales={cargarMarcasComerciales} />
    </Container>
  );
};

export default MarcasComercialesContainer;
