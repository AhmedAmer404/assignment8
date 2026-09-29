using System;


public struct DeliveryAddress
{
    public string City;
    public string Street;
    public int BuildingNumber;

    public DeliveryAddress(string city, string street, int buildingNumber)
    {
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
    }

    public string GetFullAddress()
    {
        return $"{BuildingNumber}, {Street}, {City}";
    }
}



public class Shipment
{
    private string trackingCode;
    private string description;
    private decimal weight;
    private decimal deliveryFee;



    public string TrackingCode
    {
        get { return trackingCode; }

        private set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                trackingCode = value;
            }
        }
    }

    public string Description
    {
        get { return description; }

        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                description = value;
            }
        }
    }


   
    public decimal Weight
    {
        get { return weight; }

        set
        {
            if (value > 0)
            {
                weight = value;
            }
        }
    }


    public decimal DeliveryFee
    {
        get { return deliveryFee; }

        private set
        {
            if (value > 0)
            {
                deliveryFee = value;
            }
        }
    }


    public DeliveryAddress Destination { get; set; }




    public virtual decimal EstimatedCost
    {
        get
        {
            return DeliveryFee + (Weight * 5);
        }
    }

    public Shipment(string trackingCode)
    {
        TrackingCode = trackingCode;

        Description = "Unknown";
        Weight = 1;
        DeliveryFee = 50;

        Destination = new DeliveryAddress(
            "Unknown",
            "Unknown",
            0
        );
    }


    public Shipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
    {
        TrackingCode = trackingCode;
        Description = description;
        Weight = weight;
        DeliveryFee = deliveryFee;
        Destination = destination;
    }



    public void UpdateDeliveryFee(decimal newFee)
    {
        if (newFee > 0)
        {
            DeliveryFee = newFee;
        }
    }


    public void UpdateWeight(decimal newWeight)
    {
        if (newWeight > 0)
        {
            Weight = newWeight;
        }
    }



    public void UpdateWeight(
        decimal newWeight,
        decimal extraPackingWeight)
    {
        decimal totalWeight = newWeight + extraPackingWeight;

        if (totalWeight > 0)
        {
            Weight = totalWeight;
        }
    }



    public virtual void PrintShipment()
    {
        Console.WriteLine("--------------------------------------");
        Console.WriteLine($"Tracking Code : {TrackingCode}");
        Console.WriteLine($"Description   : {Description}");
        Console.WriteLine($"Weight        : {Weight}");
        Console.WriteLine($"Delivery Fee  : {DeliveryFee}");
        Console.WriteLine($"Destination   : {Destination.GetFullAddress()}");
        Console.WriteLine($"Estimated Cost: {EstimatedCost}");
        Console.WriteLine("--------------------------------------");
    }
}


public class StandardShipment : Shipment
{
    public StandardShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)

        : base(
            trackingCode,
            description,
            weight,
            deliveryFee,
            destination)
    {
    }


    // Override PrintShipment
    public override void PrintShipment()
    {
        Console.WriteLine("\n===== STANDARD SHIPMENT =====");

        base.PrintShipment();
    }
}


public class ExpressShipment : Shipment
{
    private decimal extraFee;


    public decimal ExtraFee
    {
        get { return extraFee; }

        set
        {
            if (value >= 0)
            {
                extraFee = value;
            }
        }
    }


    public ExpressShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        decimal extraFee)

        : base(
            trackingCode,
            description,
            weight,
            deliveryFee,
            destination)
    {
        ExtraFee = extraFee;
    }


    public override decimal EstimatedCost
    {
        get
        {
            return DeliveryFee
                   + (Weight * 5)
                   + ExtraFee;
        }
    }


    public override void PrintShipment()
    {
        Console.WriteLine("\n===== EXPRESS SHIPMENT =====");

        base.PrintShipment();

        Console.WriteLine($"Extra Fee     : {ExtraFee}");
    }
}


public class InternationalShipment : Shipment
{
    private string destinationCountry;
    private decimal customsFee;


    public string DestinationCountry
    {
        get { return destinationCountry; }

        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                destinationCountry = value;
            }
        }
    }

    public decimal CustomsFee
    {
        get { return customsFee; }

        set
        {
            if (value >= 0)
            {
                customsFee = value;
            }
        }
    }

    public InternationalShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        string destinationCountry,
        decimal customsFee)

        : base(
            trackingCode,
            description,
            weight,
            deliveryFee,
            destination)
    {
        DestinationCountry = destinationCountry;
        CustomsFee = customsFee;
    }


    public override decimal EstimatedCost
    {
        get
        {
            return DeliveryFee
                   + (Weight * 5)
                   + CustomsFee;
        }
    }

    public override void PrintShipment()
    {
        Console.WriteLine("\n===== INTERNATIONAL SHIPMENT =====");

        base.PrintShipment();

        Console.WriteLine($"Destination Country : {DestinationCountry}");
        Console.WriteLine($"Customs Fee         : {CustomsFee}");
    }
}

public class DeliveryCenter
{
    private Shipment[] shipments;


    public string CenterName { get; set; }


    public DeliveryCenter(string centerName)
    {
        CenterName = centerName;

        // Maximum 20 shipments
        shipments = new Shipment[20];
    }



    public Shipment this[int index]
    {
        get
        {
            if (index >= 0 && index < shipments.Length)
            {
                return shipments[index];
            }

            return null;
        }

        set
        {
            if (index >= 0 && index < shipments.Length)
            {
                shipments[index] = value;
            }
        }
    }


    public Shipment this[string trackingCode]
    {
        get
        {
            for (int i = 0; i < shipments.Length; i++)
            {
                if (shipments[i] != null &&
                    shipments[i].TrackingCode == trackingCode)
                {
                    return shipments[i];
                }
            }

            return null;
        }
    }


    public bool AddShipment(Shipment shipment)
    {
        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] == null)
            {
                shipments[i] = shipment;

                return true;
            }
        }

        return false;
    }

    public bool RemoveShipment(string trackingCode)
    {
        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] != null &&
                shipments[i].TrackingCode == trackingCode)
            {
                shipments[i] = null;

                return true;
            }
        }

        return false;
    }

    public void PrintAllShipments()
    {
        Console.WriteLine("\n======================================");
        Console.WriteLine($"Delivery Center: {CenterName}");
        Console.WriteLine("======================================");

        bool found = false;

        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] != null)
            {
                shipments[i].PrintShipment();

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No shipments available.");
        }
    }
}



