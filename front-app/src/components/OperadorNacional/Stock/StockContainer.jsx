import { useState, useEffect, useContext } from "react";
import { UserContext } from "@/components/Context/UserContext";
import { Container } from "@mui/system";
import axios from "axios";
import allUrls from "@/services/backend_url";
import { Table, TableContainer, TableHead, TableRow, Paper, TableCell, TableBody } from "@mui/material";
import CustomLoader from "@/components/utils/CustomLoader";
const StockContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [stock, setStock] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);

  useEffect(() => {
    axios
      .get(`${allUrls.visualizarStock}?emailOperadorNacional=${userSesion.email}`)
      .then((response) => {
        setStock(response.data);
      })
      .catch(() => alert("Ocurrio un error"))
      .finally(() => setEstaCargando(false));
  }, []);
  return (
    <Container>
      {estaCargando && <CustomLoader />}
      <TableContainer component={Paper} sx={{ marginTop: 1 }}>
        <Table sx={{ minWidth: 650 }} size="small">
          <TableHead sx={{ backgroundColor: "#2E7994" }}>
            <TableRow>
              <TableCell sx={{ color: "white", fontWeight: 600 }}>Descripción</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {stock.map((element, index) => (
              <TableRow key={index}>
                <TableCell>{element.descripcion}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default StockContainer;
