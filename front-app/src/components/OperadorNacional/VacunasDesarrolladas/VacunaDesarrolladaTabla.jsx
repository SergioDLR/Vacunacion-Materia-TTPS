import { TableContainer, TableRow, TableCell, TableBody, Table, Paper, TableHead } from "@mui/material";
import VacunaDesarrollada from "./VacunaDesarrollada";

const VacunaDesarrolladaTabla = ({
  vacunasDesarrolladas = [],
  cargarVacunasDesarrolladas,
  vacunasDesarrolladasEliminadas,
  mostrarEliminadas,
}) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }}>Vacuna</TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Marca comercial
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Días demora
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Precio
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Acciones
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {vacunasDesarrolladas.map((element, index) => (
            <VacunaDesarrollada cargarVacunasDesarrolladas={cargarVacunasDesarrolladas} vacuna={element} key={index} />
          ))}

          {mostrarEliminadas &&
            vacunasDesarrolladasEliminadas.map((element, index) => (
              <VacunaDesarrollada
                cargarVacunasDesarrolladas={cargarVacunasDesarrolladas}
                vacuna={element}
                key={index}
                eliminada={true}
              />
            ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default VacunaDesarrolladaTabla;
