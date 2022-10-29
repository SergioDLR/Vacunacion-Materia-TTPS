import { TableContainer, TableRow, TableCell, TableBody, Table, Paper, TableHead } from "@mui/material";
import Distribucion from "./Distribucion";
const TableDistribuciones = ({ distribuciones }) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }}>Jurisdicción</TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Lote
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Fecha de entrega
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Cantidad de vacunas
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Aplicadas
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Vencidas
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {distribuciones.map((element, index) => (
            <Distribucion distribucion={element} key={index} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default TableDistribuciones;
