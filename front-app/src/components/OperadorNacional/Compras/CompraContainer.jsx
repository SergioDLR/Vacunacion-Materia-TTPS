import { Box, Container } from "@mui/system";
import { useAlert } from "react-alert";
import CustomButton from "@/components/utils/CustomButtom";
import CustomModal from "@/components/utils/Modal";
import { useEffect, useState, useContext } from "react";
import FormCompra from "./RegistrarCompra/FormCompra";
import { UserContext } from "@/components/Context/UserContext";
import axios from "axios";
import CompraTabla from "./CompraTable";
import allUrls from "@/services/backend_url";
import CustomLoader from "@/components/utils/CustomLoader";
import SelectVacunasDesarrolladas from "./RegistrarCompra/SelectVacunasDesarrolladas";
const CompraContainer = () => {
  const [open, setOpen] = useState(false);
  const [comprasCreadas, setComprasCreadas] = useState([]);
  const { userSesion } = useContext(UserContext);
  const alert = useAlert();
  const [estaCargando, setEstaCargando] = useState(true);
  const [vacunaDesarrolladaSeleccionada, setVacunasDesarrolladasSeleccionada] = useState(0);
  useEffect(() => {
    cargarCompras();
  }, []);

  const cargarCompras = (id = 0) => {
    try {
      setEstaCargando(true);
      let url = `${allUrls.visualizarCompra}?emailOperadorNacional=${userSesion.email}`;
      if (id !== 0)
        url = `${allUrls.visualizarCompra}?emailOperadorNacional=${userSesion.email}&idVacunaDesarrollada=${id}`;
      axios
        .get(url)
        .then((response) => {
          if (response?.data?.estadoTransaccion === "Aceptada") {
            setComprasCreadas(response?.data?.compras);
          } else {
            alert.error(`Ocurrieron errores: ${response?.data?.errores}`);
          }
        })
        .catch((error) => {
          alert.error(`Ocurrieron errores: ${error}`);
        })
        .finally(() => {
          setEstaCargando(false);
        });
    } catch (e) {
      setEstaCargando(false);
      alert.error(`Ocurrio un error del lado del servidor`);
    }
  };
  const cronEjecucion = () => {
    axios.get(`${allUrls.cron}?emailOperadorNacional=${userSesion.email}`).then((response) => {
      cargarCompras();
      alert.success("Se actualizaron las compras y los lotes vencidos");
    });
  };
  return (
    <Container>
      <CustomButton
        variant={"outlined"}
        color="success"
        textColor={"green"}
        sx={{ marginTop: 2 }}
        onClick={cronEjecucion}
      >
        Ejecutra cron
      </CustomButton>
      {estaCargando && <CustomLoader />}

      <CustomModal title={"comprar"} displayButton={false} open={open} setOpen={setOpen}>
        <FormCompra setOpen={setOpen} alert={alert} cargarCompras={cargarCompras} />
        <CustomButton
          sx={{ marginRight: 1 }}
          textColor={"#red"}
          color={"error"}
          variant={"outlined"}
          onClick={() => setOpen(false)}
        >
          Cancelar
        </CustomButton>
      </CustomModal>

      <CustomButton
        sx={{ marginTop: 1 }}
        color={"info"}
        textColor={"#2E7994"}
        variant={"outlined"}
        onClick={() => setOpen(true)}
      >
        Comprar
      </CustomButton>
      <Box sx={{ marginTop: 1, maxWidth: "50%" }}>
        <h3>Filtrar por vacuna</h3>
        <SelectVacunasDesarrolladas
          urls={allUrls}
          userMail={userSesion.email}
          vacunaDesarrolladaSeleccionada={vacunaDesarrolladaSeleccionada}
          setVacunasDesarrolladasSeleccionada={setVacunasDesarrolladasSeleccionada}
          efectoSecundario={cargarCompras}
          opcionTodas={true}
        />
      </Box>
      <CompraTabla compras={comprasCreadas} />
    </Container>
  );
};

export default CompraContainer;
